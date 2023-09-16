import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from 'src/app/auth/login-request';
import { LoginResult } from 'src/app/auth/login-result';
import { environment } from 'src/environments/environment';
import { Observable,tap,Subject } from 'rxjs';
import { ErrorService } from 'src/app/common/error.service';
import { AbstractControl } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private tokenKey: string = "token";

  private _authStatus = new Subject<boolean>();
  public authStatus = this._authStatus.asObservable();

  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  init(): void {
    if (this.isAuthenticated())
      this.setAuthStatus(true);
  }

  login(item: LoginRequest): Observable<LoginResult> {
    var url = environment.baseUrl + "api/Account/Login";

    var params = new HttpParams()
      .set("email", item.email.toString())
      .set("password", item.password.toString());

    return this.http.get<LoginResult>(url, { params })
      .pipe(tap(loginResult => {
        if (loginResult.success && loginResult.token) {
          localStorage.setItem(this.tokenKey, loginResult.token);
          this.setAuthStatus(true);
        }
      }));
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.setAuthStatus(false);
  }

  getErrors(
    control: AbstractControl,
    displayName: string,
    customMessages: { [key: string]: string } | null = null
  ): string[] {
    return this.errorService.getErrors(control, displayName, customMessages);
  }

  private setAuthStatus(isAuthenticated: boolean): void {
    this._authStatus.next(isAuthenticated);
  }

  constructor(protected http: HttpClient,private errorService: ErrorService) { }
}
