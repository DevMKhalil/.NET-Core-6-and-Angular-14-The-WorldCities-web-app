import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { CitiesComponent } from 'src/app/cities/cities.component';
import { CountriesComponent } from 'src/app/countries/countries.component';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [HomeComponent, NavMenuComponent, RouterModule, CitiesComponent, CountriesComponent]
})
export class AppComponent {

  constructor() {}

  title = 'WorldCities';
}

