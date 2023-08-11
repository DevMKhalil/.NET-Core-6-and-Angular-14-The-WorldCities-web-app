import { Injectable } from '@angular/core';
//import { Country } from 'src/app/countries/country';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ApiResult } from 'src/app/common/ApiResult';

@Injectable({
  providedIn: 'root'
})
export class SharedService<T> {

  getEntityList(
    apiName: string,
    pageIndex: number,
    pageSize: number,
    sortColumn?: string,
    sortOrder?: string,
    filterColumn?: string,
    filterQuery?: string) {
    var url = environment.baseUrl + 'api/' + apiName;

    var params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString());

    if (sortColumn && sortOrder)
      params = params
        .set("sortColumn", sortColumn)
        .set("sortOrder", sortOrder);

    if (filterQuery && filterColumn)
      params = params
        .set("filterColumn", filterColumn)
        .set("filterQuery", filterQuery);

    return this.http.get<ApiResult<T>>(url, { params });
  }

  getEntity(apiName: string, id:number) {
    var url = environment.baseUrl + 'api/' + apiName + '/' + id;

    return this.http.get<T>(url);
  }

  putEntity(apiName: string,entity: T) {
    var url = environment.baseUrl + 'api/' + apiName;

    return this.http.put<T>(url, entity);
  }

  postEntity(apiName: string, entity: T) {
    var url = environment.baseUrl + 'api/' + apiName;

    return this.http.post<T>(url, entity);
  }

  constructor(private http: HttpClient) { }
}
