import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { City } from 'src/app/cities/city';
import { Subscription } from 'rxjs';
import { CitiesServiceService } from 'src/app/cities/cities-service.service';
import { StyleMaterialModule } from 'src/app/style-material/style-material.module';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  standalone: true,
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.scss'],
  imports: [StyleMaterialModule]
})
export class CitiesComponent implements OnInit, OnDestroy {

  public displayedColumns: string[] = ['id', 'name', 'lat', 'lon'];
  public cities!: MatTableDataSource<City>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  citySubscription!: Subscription;

  ngOnInit(): void {
    this.citySubscription = this.cityService.getCities().subscribe(
      {
        next: (result: City[]) => {
          this.cities = new MatTableDataSource<City>(result);
          this.cities.paginator = this.paginator;
        },
        error: err => console.error(err)
      });
  }

  ngOnDestroy(): void {
    this.citySubscription.unsubscribe();
  }

  constructor(private cityService: CitiesServiceService) { }
}
