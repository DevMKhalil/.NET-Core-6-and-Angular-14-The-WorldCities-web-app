import { Injectable } from '@angular/core';
import { City } from 'src/app/cities/city';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CitiesServiceService {
  public cities!: City[];

  getCities() {
    return this.http.get<City[]>(environment.baseUrl + 'api/Cities');
  }

  constructor(private http: HttpClient) { }
}
