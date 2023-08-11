import { Component, OnInit } from '@angular/core';
import { StyleMaterialModule } from 'src/app/style-material/style-material.module';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { City } from 'src/app/cities/city';
import { CitiesService } from 'src/app/cities/cities.service';
import { Subscription } from 'rxjs';

@Component({
  standalone: true, 
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrls: ['./city-edit.component.scss'],
  imports: [RouterModule, StyleMaterialModule]
})
export class CityEditComponent implements OnInit {

  // the view title
  title?: string;
  // the form model
  form!: FormGroup;
  // the city object to edit or create
  city?: City;

  citySubscription!: Subscription;

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl(''),
      lat: new FormControl(''),
      lon: new FormControl('')
    });

    this.loadData();
  }

  loadData() {
    // retrieve the ID from the 'id' parameter
    var idParam = this.activatedRoute.snapshot.paramMap.get('id');
    var id = idParam ? +idParam : 0;

    // fetch the city from the server

    this.citySubscription = this.cityService.getCity(id).subscribe(result => {
      this.city = result;
      this.title = "Edit - " + this.city.name;

      // update the form with the city value
      this.form.patchValue(this.city);
    }, error => console.error(error));
  }

  onSubmit() {
    debugger

    var city = this.city;
    if (city) {
      city.name = this.form.controls['name'].value;
      city.lat = +this.form.controls['lat'].value;
      city.lon = +this.form.controls['lon'].value;

      this.citySubscription = this.cityService.putCity(city)
        .subscribe(result => {
          console.log("City " + city!.id + " has been updated.");
          // go back to cities view
          this.router.navigate(['/cities']);
        }, error => console.error(error));
    }
  }

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private cityService: CitiesService) { }
}
