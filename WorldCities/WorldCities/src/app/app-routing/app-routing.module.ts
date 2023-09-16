import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/auth/auth.guard';

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
    loadComponent: () => import('src/app/cities/city-edit/city-edit.component').then(mod => mod.CityEditComponent),
    canActivate: [AuthGuard]
  }
  ,
  {
    path: 'city',
    loadComponent: () => import('src/app/cities/city-edit/city-edit.component').then(mod => mod.CityEditComponent),
    canActivate: [AuthGuard]
  }
  ,
  {
    path: 'countries',
    loadComponent: () => import('src/app/countries/countries.component').then(mod => mod.CountriesComponent)
  }
  ,
  {
    path: 'country/:id',
    loadComponent: () => import('src/app/countries/country-edit/country-edit.component').then(mod => mod.CountryEditComponent),
    canActivate: [AuthGuard]
  }
  ,
  {
    path: 'country',
    loadComponent: () => import('src/app/countries/country-edit/country-edit.component').then(mod => mod.CountryEditComponent),
    canActivate: [AuthGuard]
  }
  ,
  {
    path: 'login',
    loadComponent: () => import('src/app/auth/login/login.component').then(mod => mod.LoginComponent)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
