import { Injectable } from '@angular/core';
import { Country } from 'src/app/countries/country';
import { SharedService } from 'src/app/common/shared.service';

@Injectable({
  providedIn: 'root'
})
export class CountriesService {

  apiName: string = 'Countries';

  getcountries(
    pageIndex: number,
    pageSize: number,
    sortColumn?: string,
    sortOrder?: string,
    filterColumn?: string,
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

  constructor(private sharedService: SharedService<Country>) { }
}
