import { Component, OnInit, OnDestroy } from '@angular/core';
import { RouterModule } from '@angular/router';
import { StyleMaterialModule } from '../style-material/style-material.module';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [RouterModule, StyleMaterialModule],
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit, OnDestroy {

  isLoggedIn: boolean = false;

  subscription!: Subscription;

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isAuthenticated();
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(["/"]);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  constructor(private authService: AuthService, private router: Router) {
    this.subscription = this.authService.authStatus.subscribe(res => this.isLoggedIn = res);
  }
}
