import { Injectable } from '@angular/core';
import { City } from 'src/app/cities/city';
import { SharedService } from 'src/app/common/shared.service';

@Injectable({
  providedIn: 'root'
})
export class CitiesService {

  apiName: string = 'Cities';

  getCities(pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string,
    filterQuery?: string) {
    return this.sharedService.getEntity(
      this.apiName,
      pageIndex,
      pageSize,
      sortColumn,
      sortOrder,
      filterColumn,
      filterQuery);
  }

  constructor(private sharedService: SharedService<City>) { }
}
