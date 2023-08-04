import { Injectable } from '@angular/core';
import { City } from 'src/app/cities/city';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ApiResult } from 'src/app/common/ApiResult';

@Injectable({
  providedIn: 'root'
})
export class CitiesServiceService {
  
  getCities(pageIndex: number, pageSize: number, sortColumn: string, sortOrder:string) {
    var url = environment.baseUrl + 'api/Cities';

    var params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("sortColumn", sortColumn)
      .set("sortOrder", sortOrder);

    return this.http.get<ApiResult<City>>(url, { params });
  }

  constructor(private http: HttpClient) { }
}
