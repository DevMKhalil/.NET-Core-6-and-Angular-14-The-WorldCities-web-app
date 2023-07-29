import { Component, OnInit, OnDestroy } from '@angular/core';
import { City } from 'src/app/cities/city';
import { Subscription } from 'rxjs';
import { CitiesServiceService } from 'src/app/cities/cities-service.service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.scss'],
  imports: [CommonModule]
})
export class CitiesComponent implements OnInit, OnDestroy {

  public cities!: City[];
  citySubscription!: Subscription;

  ngOnInit(): void {
    this.citySubscription = this.cityService.getCities().subscribe(
      {
        next: (result: City[]) => {
          this.cities = result;
        },
        error: err => console.error(err)
      });
  }

  ngOnDestroy(): void {
    this.citySubscription.unsubscribe();
  }

  constructor(private cityService: CitiesServiceService) { }
}
