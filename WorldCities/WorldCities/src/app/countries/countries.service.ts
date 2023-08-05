import { Injectable } from '@angular/core';
import { Country } from 'src/app/countries/country';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ApiResult } from 'src/app/common/ApiResult';

@Injectable({
  providedIn: 'root'
})
export class CountriesService {

  getcountries(pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string,
    filterQuery?: string) {
    var url = environment.baseUrl + 'api/Countries';

    var params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("sortColumn", sortColumn)
      .set("sortOrder", sortOrder)

    if (filterQuery)
      params = params
        .set("filterColumn", filterColumn)
        .set("filterQuery", filterQuery);

    return this.http.get<ApiResult<Country>>(url, { params });
  }

  constructor(private http: HttpClient) { }
}
