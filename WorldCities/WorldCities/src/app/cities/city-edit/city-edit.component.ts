import { Component, OnInit,OnDestroy } from '@angular/core';
import { StyleMaterialModule } from 'src/app/style-material/style-material.module';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { City } from 'src/app/cities/city';
import { CitiesService } from 'src/app/cities/cities.service';
import { Subscription } from 'rxjs';
import { Country } from 'src/app/countries/country';
import { CountriesService } from 'src/app/countries/countries.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  standalone: true, 
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrls: ['./city-edit.component.scss'],
  imports: [RouterModule, StyleMaterialModule]
})
export class CityEditComponent implements OnInit, OnDestroy {

  // the view title
  title?: string;
  // the form model
  form!: FormGroup;
  // the city object to edit or create
  city?: City;

  // the city object id, as fetched from the active route:
  // It's NULL when we're adding a new city,
  // and not NULL when we're editing an existing one.
  id?: number;

  // the countries array for the select
  countries?: Country[];

  citySubscription!: Subscription;

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl('', Validators.required),
      lat: new FormControl('', [Validators.required, Validators.pattern(/^[-]?\d+(\.\d{1,4})?$/)]),
      lon: new FormControl('', [Validators.required, Validators.pattern(/^[-]?\d+(\.\d{1,4})?$/)]),
      countryId: new FormControl('', Validators.required)
    }, null, this.isDupeCity());

    this.loadData();
  }

  loadData() {
    // load countries
    this.loadCountries();

    // retrieve the ID from the 'id' parameter
    var idParam = this.activatedRoute.snapshot.paramMap.get('id');
    this.id = idParam ? +idParam : 0;

    if (this.id) {
      // EDIT MODE

      // fetch the city from the server
      this.citySubscription = this.cityService.getCity(this.id).subscribe(result => {
        this.city = result;
        this.title = "Edit - " + this.city.name;

        // update the form with the city value
        this.form.patchValue(this.city);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE
      this.title = "Create a new City";
    }
  }

  loadCountries() {
    this.citySubscription = this.countriesService
      .getcountries(0, 9999, 'name')
      .subscribe(result => {
        this.countries = result.data;
      }, error => console.error(error));
  }

  isDupeCity(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
      var city = <City>{};
      city.id = (this.id) ? this.id : 0;
      city.name = this.form.controls['name'].value;
      city.lat = +this.form.controls['lat'].value;
      city.lon = +this.form.controls['lon'].value;
      city.countryId = +this.form.controls['countryId'].value;
      return this.cityService.isDuplicatedCity(city)
        .pipe(map(result => {
          return (result ? { isDupeCity: true } : null);
      }));
    }
  }

  onSubmit() {
    if (!this.form.valid)
      return;

    var city = (this.id) ? this.city : <City>{};
    if (city) {
      city.name = this.form.controls['name'].value;
      city.lat = +this.form.controls['lat'].value;
      city.lon = +this.form.controls['lon'].value;
      city.countryId = +this.form.controls['countryId'].value;

      if (this.id) {
        this.citySubscription = this.cityService.putCity(city)
          .subscribe(result => {
            console.log("City " + city!.id + " has been updated.");
            // go back to cities view
            this.router.navigate(['/cities']);
          }, error => console.error(error));
      }
      else {
        this.citySubscription = this.cityService.postCity(city)
          .subscribe(result => {
            console.log("City " + result.id + " has been created.");
            // go back to cities view
            this.router.navigate(['/cities']);
          }, error => console.error(error));
      }
    }
  }

  getErrors(
    control: AbstractControl,
    displayName: string,
    customMessages: { [key: string]: string } | null = null  ): string[] {
    return this.countriesService.getErrors(control, displayName, customMessages);
  }

  ngOnDestroy(): void {
    this.citySubscription.unsubscribe();
  }

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private cityService: CitiesService,
    private countriesService: CountriesService) { }
}
