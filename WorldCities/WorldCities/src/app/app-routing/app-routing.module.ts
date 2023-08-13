import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('src/app/home/home.component').then(mod => mod.HomeComponent),
    pathMatch: 'full'
  }
  ,
  {
    path: 'cities',
    loadComponent: () => import('src/app/cities/cities.component').then(mod => mod.CitiesComponent)
  }
  ,
  {
    path: 'city/:id',
    loadComponent: () => import('src/app/cities/city-edit/city-edit.component').then(mod => mod.CityEditComponent)
  }
  ,
  {
    path: 'city',
    loadComponent: () => import('src/app/cities/city-edit/city-edit.component').then(mod => mod.CityEditComponent)
  }
  ,
  {
    path: 'countries',
    loadComponent: () => import('src/app/countries/countries.component').then(mod => mod.CountriesComponent)
  }
  ,
  {
    path: 'country/:id',
    loadComponent: () => import('src/app/countries/country-edit/country-edit.component').then(mod => mod.CountryEditComponent)
  }
  ,
  {
    path: 'country',
    loadComponent: () => import('src/app/countries/country-edit/country-edit.component').then(mod => mod.CountryEditComponent)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
