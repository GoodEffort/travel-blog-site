/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface PagedPhotos {
  /** @format int32 */
  page: number;
  /** @format int32 */
  pageSize: number;
  /** @format int32 */
  totalPages: number;
  /** @format int32 */
  totalItems: number;
  photos: Photo[];
}

export interface Photo {
  /** @minLength 1 */
  id: string;
  /** @minLength 1 */
  tripName: string;
  /** @minLength 1 */
  photoName: string;
  /** @minLength 1 */
  photoDescription: string;
}
