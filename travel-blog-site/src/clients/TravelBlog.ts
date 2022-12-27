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

import type { PagedPhotos, Photo } from "./data-contracts";
import { ContentType, HttpClient, type RequestParams } from "./http-client";

export class TravelBlog<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags TravelBlog
   * @name GetPhotos
   * @request GET:/TravelBlog/FindPhotos
   */
  getPhotos = (
    query?: {
      TripName?: string;
      /**
       * @format int32
       * @default 0
       */
      page?: number;
      /**
       * @format int32
       * @default 10
       */
      pageSize?: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<PagedPhotos, any>({
      path: `/TravelBlog/FindPhotos`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags TravelBlog
   * @name GetPhoto
   * @request GET:/TravelBlog/Photo
   */
  getPhoto = (
    query?: {
      photoId?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<Photo, any>({
      path: `/TravelBlog/Photo`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags TravelBlog
   * @name CreatePhoto
   * @request POST:/TravelBlog/Photo
   */
  createPhoto = (data: Photo, params: RequestParams = {}) =>
    this.request<Photo, any>({
      path: `/TravelBlog/Photo`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
