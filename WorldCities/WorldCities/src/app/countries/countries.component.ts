import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Country } from 'src/app/countries/country';
import { Subscription } from 'rxjs';
import { CountriesService } from 'src/app/countries/countries.service';
import { StyleMaterialModule } from 'src/app/style-material/style-material.module';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ApiResult } from 'src/app/common/ApiResult';
import { RouterModule } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  standalone: true, 
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.scss'],
  imports: [RouterModule, StyleMaterialModule]
})
export class CountriesComponent implements OnInit, OnDestroy {

  public displayedColumns: string[] = ['id', 'name', 'iso2', 'iso3'];
  public countries!: MatTableDataSource<Country>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "name";
  public defaultSortOrder: "asc" | "desc" = "asc";
  defaultFilterColumn: string = "name";
  filterQuery?: string;

  filterTextChanged: Subject<string> = new Subject<string>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  countrySubscription!: Subscription;

  ngOnInit(): void {
    this.loadData();
  }

  // debounce filter text changes
  onFilterTextChanged(filterText: string) {
    if (this.filterTextChanged.observers.length === 0) {
      this.countrySubscription = this.filterTextChanged
        .pipe(debounceTime(1000), distinctUntilChanged())
        .subscribe(query => {
          this.loadData(query);
        });
    }
    this.filterTextChanged.next(filterText);
  }

  loadData(query?: string) {
    var pageEvent = new PageEvent();
    pageEvent.pageIndex = this.defaultPageIndex;
    pageEvent.pageSize = this.defaultPageSize;
    this.filterQuery = query;
    this.getData(pageEvent);
  }


  getData(e: PageEvent) {

    const sortCol = (this.sort) ? this.sort.active : this.defaultSortColumn;
    const sortDir = (this.sort) ? this.sort.direction : this.defaultSortOrder;

    this.countrySubscription = this.countriesService.getcountries(
      e.pageIndex,
      e.pageSize,
      sortCol,
      sortDir,
      this.defaultFilterColumn,
      this.filterQuery)
      .subscribe(
        {
          next: (result: ApiResult<Country>) => {
            this.paginator.length = result.totalCount;
            this.paginator.pageIndex = result.pageIndex;
            this.paginator.pageSize = result.pageSize;
            this.countries = new MatTableDataSource<Country>(result.data);
          },
          error: err => console.error(err)
        });
  }

  ngOnDestroy(): void {
    this.countrySubscription.unsubscribe();
  }

  constructor(private countriesService: CountriesService) { }
}
