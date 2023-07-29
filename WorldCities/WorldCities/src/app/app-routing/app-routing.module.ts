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
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
