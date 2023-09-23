import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { CitiesComponent } from 'src/app/cities/cities.component';
import { CountriesComponent } from 'src/app/countries/countries.component';
import { AuthService } from 'src/app/auth/auth.service';
import { ConnectionService, ConnectionState } from 'ng-connection-service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [CommonModule,HomeComponent, NavMenuComponent, RouterModule, CitiesComponent, CountriesComponent]
})
export class AppComponent implements OnInit {

  title = 'WorldCities';

  hasNetworkConnection: boolean = true;
  hasInternetAccess: boolean = true;

 

  ngOnInit(): void {
    this.authService.init();
  }

  public isOnline() {
    return this.hasNetworkConnection && this.hasInternetAccess;
  }

  constructor(private authService: AuthService, private connectionService: ConnectionService) {
    this.connectionService.monitor().subscribe((currentState: ConnectionState) => {
      this.hasNetworkConnection = currentState.hasNetworkConnection; this.hasInternetAccess = currentState.hasInternetAccess;
    });
  }
}

