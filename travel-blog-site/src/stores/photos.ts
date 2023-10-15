import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { Photo } from '@/clients/data-contracts'
import { TravelBlog } from '@/clients/TravelBlog'
import { photosList as icelandList } from './iceland-photos-list'
import { apiURL } from '@/site-info'

// do security stuff here if I add that later
const TravelBlogAPI = new TravelBlog({
    baseURL: apiURL,
});

export const usePhotosStore = defineStore('photos', () => {
    const photos = ref<Photo[]>([]);
    const currentPage = ref(0);
    const totalPageCount = ref(0);
    const currentPageSize = ref(25);
    const totalRecordCount = ref(0);

    const loading = ref(false);

    async function getPhotos(TripName: string, page: number, pageSize: number = 10) {
        loading.value = true;

        let error: Error | undefined = undefined;
        const response = await TravelBlogAPI.getPhotos({ TripName, page, pageSize });

        if (response.status === 200) {
            if (response.data.totalItems === 0) {
                error = new Error(`No photos found for ${TripName}`);
                photos.value = [];
            } else if (response.data.photos.length === 0) {
                error = new Error(`No more photos found for ${TripName} on page ${page}`);
                photos.value = [];
            } else {
                photos.value = response.data.photos;
                currentPage.value = response.data.page;
                totalPageCount.value = response.data.totalPages;
                currentPageSize.value = response.data.pageSize;
                totalRecordCount.value = response.data.totalItems;
            }
        }
        else {
            error = new Error(`Error getting photos for ${TripName} on page ${page}: ${response.statusText}`);
            photos.value = [];
        }
        loading.value = false;
        if (error) throw error;
    }

    async function generatePhotos(TripName: string) {
        loading.value = true;
        for (const photoId of icelandList) {
            const getReponse = await TravelBlogAPI.getPhoto({ photoId });
            if (getReponse.status !== 200) {
                const newPhoto: Photo = {
                    id: photoId,
                    tripName: TripName,
                    photoDescription: 'Auto Generated Description, photo: ' + photoId,
                    photoName: photoId,
                };

                try {
                    const createResponse = await TravelBlogAPI.createPhoto(newPhoto);
                    if (createResponse.status !== 201) {
                        console.error(`Error creating photo ${photoId}: ${createResponse.statusText}`);
                    } else {
                        console.log(`Created photo ${photoId}`);
                    }
                } catch (error) {
                    // probably a duplicate
                    console.error(`Error creating photo ${photoId}: ${error}`);
                }
            }
        }
        loading.value = false;
    }

    async function clearPhotos() {
        photos.value = [];
        currentPage.value = 0;
        totalPageCount.value = 0;
        totalRecordCount.value = 0;
    }

    return {
        photos,
        currentPage,
        totalPageCount,
        totalRecordCount,
        currentPageSize,
        loading,
        getPhotos,
        generatePhotos,
        clearPhotos,
    };
});