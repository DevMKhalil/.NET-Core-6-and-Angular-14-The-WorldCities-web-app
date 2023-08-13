import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { StyleMaterialModule } from 'src/app/style-material/style-material.module';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { Subscription } from 'rxjs';
import { Country } from 'src/app/countries/country';
import { CountriesService } from 'src/app/countries/countries.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-country-edit',
  standalone: true,
  imports: [RouterModule, StyleMaterialModule],
  templateUrl: './country-edit.component.html',
  styleUrls: ['./country-edit.component.scss']
})
export class CountryEditComponent implements OnInit, OnDestroy {

  countrySubscription!: Subscription;

  // the view title
  title?: string;

  // the form model
  form!: FormGroup;

  // the country object to edit or create
  country?: Country;

  // the country object id, as fetched from the active route:
  // It's NULL when we're adding a new country,
  // and not NULL when we're editing an existing one.
  id?: number;

  // the countries array for the select
  countries?: Country[];

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', Validators.required, this.isDupeField("name")],
      iso2: ['', [Validators.required, Validators.pattern(/^[a-zA-Z]{2}$/)], this.isDupeField("iso2")],
      iso3: ['', [Validators.required, Validators.pattern(/^[a-zA-Z]{3}$/)], this.isDupeField("iso3")]
    });

    this.loadData();
  }

  loadData() {

    // retrieve the ID from the 'id' parameter
    var idParam = this.activatedRoute.snapshot.paramMap.get('id');
    this.id = idParam ? +idParam : 0;

    if (this.id) {
      // EDIT MODE
      // fetch the country from the server
      this.countrySubscription = this.countriesService.getCountry(this.id).subscribe(result => {
        this.country = result;
        this.title = "Edit - " + this.country.name;

        // update the form with the country value
        this.form.patchValue(this.country);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE
      this.title = "Create a new Country";
    }
  }

  onSubmit() {
    if (!this.form.valid)
      return;

    var country = (this.id) ? this.country : <Country>{};
    if (country) {
      country.name = this.form.controls['name'].value;
      country.iso2 = this.form.controls['iso2'].value;
      country.iso3 = this.form.controls['iso3'].value;

      if (this.id) {
        this.countrySubscription = this.countriesService.putCountry(country)
          .subscribe(result => {
            console.log("Country " + country!.id + " has been updated.");
            // go back to cities view
            this.router.navigate(['/countries']);
          }, error => console.error(error));
      }
      else {
        this.countrySubscription = this.countriesService.postCountry(country)
          .subscribe(result => {
            console.log("Country " + result.id + " has been created.");
            // go back to cities view
            this.router.navigate(['/countries']);
          }, error => console.error(error));
      }
    }
  }

  isDupeField(fieldName: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{
      [key: string]: any
    } | null> => {
      return this.countriesService
        .isDuplicatedField(fieldName, this.id, control.value)
          .pipe(map(result => {
            return (result ? { isDupeField: true } : null);
          }));
    }
  }

  ngOnDestroy(): void {
    this.countrySubscription.unsubscribe();
  }

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private countriesService: CountriesService
  ) { }
}
