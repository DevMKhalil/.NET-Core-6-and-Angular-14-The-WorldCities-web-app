import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [HomeComponent, NavMenuComponent, RouterModule]
})
export class AppComponent {
  //public forecasts?: WeatherForecast[];

  constructor(/*http: HttpClient*/) {
    //http.get<WeatherForecast[]>(environment.baseUrl + 'api/weatherforecast').subscribe(result => {
    //  this.forecasts = result;
    //}, error => console.error(error));
  }

  title = 'WorldCities';
}

//interface WeatherForecast {
//  date: string;
//  temperatureC: number;
//  temperatureF: number;
//  summary: string;
//}
