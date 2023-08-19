import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StyleMaterialModule } from 'src/app/style-material/style-material.module';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';
import { CitiesComponent } from 'src/app/cities/cities.component';
import { City } from './city';
import { CitiesService } from 'src/app/cities/cities.service';
import { ApiResult } from 'src/app/common/ApiResult';


describe('CitiesComponent', () => {
  let component: CitiesComponent;
  let fixture: ComponentFixture<CitiesComponent>;

  beforeEach(async () => {

    // Create a mock cityService object with a mock 'getData' method
    let cityService = jasmine.createSpyObj<CitiesService>('CitiesService', ['getCitiesList']);

    // Configure the 'getData' spy method
    cityService.getCitiesList.and.returnValue(
      // return an Observable with some test data
      of<ApiResult<City>>(<ApiResult<City>>{
        data: [
          <City>{
            name: 'TestCity1',
            id: 1,
            lat: 1,
            lon: 1,
            countryId: 1,
            countryName: 'TestCountry1'
          },
          <City>{
            name: 'TestCity2',
            id: 2,
            lat: 1,
            lon: 1,
            countryId: 1,
            countryName: 'TestCountry1'
          },
          <City>{
            name: 'TestCity3',
            id: 3,
            lat: 1,
            lon: 1,
            countryId: 1,
            countryName: 'TestCountry1'
          }
        ],
        totalCount: 3,
        pageIndex: 0,
        pageSize: 10,
        totalPages: 1,
        hasPreviousPage: false,
        hasNextPage: false
      })
    );

    await TestBed.configureTestingModule({
      //declarations: [CitiesComponent],
      imports: [
        BrowserAnimationsModule,
        StyleMaterialModule,
        RouterTestingModule
      ],
      providers: [
        {
          provide: CitiesService,
          useValue: cityService
        }
      ]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitiesComponent);
    component = fixture.componentInstance;

    component.paginator = jasmine.createSpyObj(
      "MatPaginator", ["length", "pageIndex", "pageSize"]
    );

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display a "Cities" title', () => {
    let title = fixture.nativeElement
      .querySelector('h1');
    expect(title.textContent).toEqual('Cities');
  });

  it('should contain a table with a list of one or more cities', () => {
    let table = fixture.nativeElement
      .querySelector('table.mat-table');
    let tableRows = table
      .querySelectorAll('tr.mat-row');
    expect(tableRows.length).toBeGreaterThan(0);
  });

})
