import { Injectable } from '@angular/core';
import { City } from 'src/app/cities/city';
import { SharedService } from 'src/app/common/shared.service';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CitiesService {

  apiName: string = 'Cities';

  getCitiesList(pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string,
    filterQuery?: string) {
    return this.sharedService.getEntityList(
      this.apiName,
      pageIndex,
      pageSize,
      sortColumn,
      sortOrder,
      filterColumn,
      filterQuery);
  }

  getCity(id:number) {
    return this.sharedService.getEntity(this.apiName, id);
  }

  putCity(city: City) {
    return this.sharedService.putEntity(this.apiName, city);
  }

  postCity(city: City) {
    return this.sharedService.postEntity(this.apiName, city);
  }

  isDuplicatedCity(city: City) {
    var url = environment.baseUrl + 'api/Cities/IsDupeCity';

    var params = new HttpParams()
      .set("id", city.id.toString())
      .set("countryId", city.countryId.toString())
      .set("lat", city.lat.toString())
      .set("lon", city.lon.toString())
      .set("name", city.name)

    return this.http.get<boolean>(url, { params });
  }

  constructor(private sharedService: SharedService<City>, private http: HttpClient) { }
}
