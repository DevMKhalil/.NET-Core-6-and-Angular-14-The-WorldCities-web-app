import { Injectable } from '@angular/core';
import { Country } from 'src/app/countries/country';
import { SharedService } from 'src/app/common/shared.service';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AbstractControl } from '@angular/forms';

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

  getCountry(id: number) {
    return this.sharedService.getEntity(this.apiName, id);
  }

  putCountry(country: Country) {
    return this.sharedService.putEntity(this.apiName, country);
  }

  postCountry(country: Country) {
    return this.sharedService.postEntity(this.apiName, country);
  }

  isDuplicatedField(fieldName: string, id: number| undefined , value: any){
    var params = new HttpParams()
      .set("countryId", (id) ? id.toString() : "0")
      .set("fieldName", fieldName)
      .set("fieldValue", value);
    var url = environment.baseUrl + 'api/Countries/IsDupeField';
    return this.http.get<boolean>(url, { params });
  }

  getErrors(
    control: AbstractControl,
    displayName: string,
    customMessages: { [key: string]: string } | null = null
  ): string[] {
    return this.sharedService.getErrors(control, displayName, customMessages);
  }

  constructor(private sharedService: SharedService<Country>, private http: HttpClient) { }
}
