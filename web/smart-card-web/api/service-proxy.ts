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

export interface BaseCardDto {
  /** @format uuid */
  id?: string | null;
  key?: string | null;
  front?: string | null;
  back?: string | null;
}

export enum CardRating {
  Again = "Again",
  Hard = "Hard",
  Good = "Good",
  Easy = "Easy",
}

export interface CreateTopicInput {
  name?: string | null;
  description?: string | null;
  cards?: BaseCardDto[] | null;
}

export interface DateOnly {
  /** @format int32 */
  year?: number;
  /** @format int32 */
  month?: number;
  /** @format int32 */
  day?: number;
  dayOfWeek?: DayOfWeek;
  /** @format int32 */
  dayOfYear?: number;
  /** @format int32 */
  dayNumber?: number;
}

export enum DayOfWeek {
  Sunday = "Sunday",
  Monday = "Monday",
  Tuesday = "Tuesday",
  Wednesday = "Wednesday",
  Thursday = "Thursday",
  Friday = "Friday",
  Saturday = "Saturday",
}

export enum Gender {
  Male = "Male",
  Female = "Female",
  Others = "Others",
}

export interface GetDueCardsOutput {
  /** @format uuid */
  id?: string | null;
  key?: string | null;
  front?: string | null;
  back?: string | null;
  imagePath?: string | null;
  description?: string | null;
}

export interface GetTopicByIdOutput {
  /** @format uuid */
  id?: string;
  name?: string | null;
  description?: string | null;
  cards?: BaseCardDto[] | null;
  canDoExercise?: boolean;
}

export interface GetTopicStatisticsOutput {
  /** @format uuid */
  id?: string;
  name?: string | null;
  /** @format int32 */
  totalCards?: number;
  /** @format int32 */
  newCards?: number;
  /** @format int32 */
  learningCards?: number;
  /** @format int32 */
  reviewCards?: number;
  /** @format int32 */
  dueCards?: number;
  /** @format int32 */
  totalReviews?: number;
  /** @format double */
  averageStability?: number;
  avatar?: string | null;
}

export interface LoginOutput {
  accessToken?: string | null;
}

export interface ReviseCardCommand {
  /** @format uuid */
  cardId?: string;
  rating?: CardRating;
}

export enum Score {
  Forgotten = "Forgotten",
  Poor = "Poor",
  Moderate = "Moderate",
  Good = "Good",
  Perfect = "Perfect",
}

export interface ScoreInput {
  /** @format uuid */
  id?: string;
  score?: Score;
}

export interface UpdateCardDto {
  /** @format uuid */
  id?: string | null;
  key?: string | null;
  front?: string | null;
  back?: string | null;
  isDeleted?: boolean;
}

export interface UpdateTopicInput {
  /** @format uuid */
  id?: string;
  name?: string | null;
  description?: string | null;
  cards?: UpdateCardDto[] | null;
}

export interface UserModel {
  /** @format uuid */
  id?: string;
  email?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  dateOfBirth?: DateOnly;
  gender?: Gender;
  address?: string | null;
}

import type { AxiosInstance, AxiosRequestConfig, AxiosResponse, HeadersDefaults, ResponseType } from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType;
  /** request body */
  body?: unknown;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
  secure?: boolean;
  format?: ResponseType;
}

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance;
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private secure?: boolean;
  private format?: ResponseType;

  constructor({ securityWorker, secure, format, ...axiosConfig }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({ ...axiosConfig, baseURL: axiosConfig.baseURL || "" });
    this.secure = secure;
    this.format = format;
    this.securityWorker = securityWorker;
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected mergeRequestParams(params1: AxiosRequestConfig, params2?: AxiosRequestConfig): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method);

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method && this.instance.defaults.headers[method.toLowerCase() as keyof HeadersDefaults]) || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === "object" && formItem !== null) {
      return JSON.stringify(formItem);
    } else {
      return `${formItem}`;
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    if (input instanceof FormData) {
      return input;
    }
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key];
      const propertyContent: any[] = property instanceof Array ? property : [property];

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File;
        formData.append(key, isFileType ? formItem : this.stringifyFormItem(formItem));
      }

      return formData;
    }, new FormData());
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    ...params
  }: FullRequestParams): Promise<AxiosResponse<T>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const responseFormat = format || this.format || undefined;

    if (type === ContentType.FormData && body && body !== null && typeof body === "object") {
      body = this.createFormData(body as Record<string, unknown>);
    }

    if (type === ContentType.Text && body && body !== null && typeof body !== "string") {
      body = JSON.stringify(body);
    }

    return this.instance.request({
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type ? { "Content-Type": type } : {}),
      },
      params: query,
      responseType: responseFormat,
      data: body,
      url: path,
    });
  };
}

/**
 * @title SmartCard.Api
 * @version 1.0
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  auth = {
    /**
     * No description
     *
     * @tags Auth
     * @name GoogleGenerateLoginUrlList
     * @request GET:/Auth/google/generate-login-url
     * @secure
     */
    googleGenerateLoginUrlList: (params: RequestParams = {}) =>
      this.request<string, any>({
        path: `/Auth/google/generate-login-url`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Auth
     * @name GoogleExchangeCodeList
     * @request GET:/Auth/google/exchange-code
     * @secure
     */
    googleExchangeCodeList: (
      query?: {
        code?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<LoginOutput, any>({
        path: `/Auth/google/exchange-code`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),
  };
  card = {
    /**
     * No description
     *
     * @tags Card
     * @name ReviseCreate
     * @request POST:/Card/revise
     * @secure
     */
    reviseCreate: (data: ReviseCardCommand, params: RequestParams = {}) =>
      this.request<boolean, any>({
        path: `/Card/revise`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Card
     * @name GetCard
     * @request GET:/Card/due
     * @secure
     */
    getCard: (
      query?: {
        /** @format uuid */
        topicId?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetDueCardsOutput[], any>({
        path: `/Card/due`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),
  };
  topic = {
    /**
     * No description
     *
     * @tags Topic
     * @name StatisticsList
     * @request GET:/Topic/statistics
     * @secure
     */
    statisticsList: (params: RequestParams = {}) =>
      this.request<GetTopicStatisticsOutput[], any>({
        path: `/Topic/statistics`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Topic
     * @name TopicCreate
     * @request POST:/Topic
     * @secure
     */
    topicCreate: (data: CreateTopicInput, params: RequestParams = {}) =>
      this.request<string, any>({
        path: `/Topic`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Topic
     * @name TopicDetail
     * @request GET:/Topic/{id}
     * @secure
     */
    topicDetail: (id: string, params: RequestParams = {}) =>
      this.request<GetTopicByIdOutput, any>({
        path: `/Topic/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Topic
     * @name TopicUpdate
     * @request PUT:/Topic/{id}
     * @secure
     */
    topicUpdate: (id: string, data: UpdateTopicInput, params: RequestParams = {}) =>
      this.request<string, any>({
        path: `/Topic/${id}`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Topic
     * @name ExerciseDetail
     * @request GET:/Topic/{id}/$exercise
     * @secure
     */
    exerciseDetail: (id: string, params: RequestParams = {}) =>
      this.request<GetDueCardsOutput[], any>({
        path: `/Topic/${id}/$exercise`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Topic
     * @name ScorePartialUpdate
     * @request PATCH:/Topic/{id}/$score
     * @secure
     */
    scorePartialUpdate: (id: string, data: ScoreInput[], params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/Topic/${id}/$score`,
        method: "PATCH",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),
  };
  user = {
    /**
     * No description
     *
     * @tags User
     * @name UserDetail
     * @request GET:/User/{email}
     * @secure
     */
    userDetail: (email: string, params: RequestParams = {}) =>
      this.request<UserModel, any>({
        path: `/User/${email}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),
  };
}
