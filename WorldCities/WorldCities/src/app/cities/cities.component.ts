import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { City } from 'src/app/cities/city';
import { Subscription } from 'rxjs';
import { CitiesServiceService } from 'src/app/cities/cities-service.service';
import { StyleMaterialModule } from 'src/app/style-material/style-material.module';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ApiResult } from 'src/app/common/ApiResult';

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

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "name";
  public defaultSortOrder: "asc" | "desc" = "asc";

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  citySubscription!: Subscription;

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    var pageEvent = new PageEvent();
    pageEvent.pageIndex = this.defaultPageIndex;
    pageEvent.pageSize = this.defaultPageSize;
    this.getData(pageEvent);
  }

  getData(e: PageEvent) {

    const sortCol = (this.sort) ? this.sort.active : this.defaultSortColumn;
    const sortDir = (this.sort) ? this.sort.direction : this.defaultSortOrder;

    this.citySubscription = this.cityService.getCities(e.pageIndex, e.pageSize, sortCol, sortDir).subscribe(
      {
        next: (result: ApiResult<City>) => {
          this.paginator.length = result.totalCount;
          this.paginator.pageIndex = result.pageIndex;
          this.paginator.pageSize = result.pageSize;
          this.cities = new MatTableDataSource<City>(result.data);
        },
        error: err => console.error(err)
      });
  }

  ngOnDestroy(): void {
    this.citySubscription.unsubscribe();
  }

  constructor(private cityService: CitiesServiceService) { }
}
