import { Injectable } from '@angular/core';
import { City } from 'src/app/cities/city';
import { SharedService } from 'src/app/common/shared.service';

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

  constructor(private sharedService: SharedService<City>) { }
}
