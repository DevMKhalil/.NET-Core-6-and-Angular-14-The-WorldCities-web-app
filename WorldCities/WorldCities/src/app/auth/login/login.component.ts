import { Component, OnInit } from '@angular/core';
import { StyleMaterialModule } from 'src/app/style-material/style-material.module';
import { LoginResult } from 'src/app/auth/login-result';
import { LoginRequest } from 'src/app/auth/login-request';
import { AuthService } from 'src/app/auth/auth.service';
import { /*ActivatedRoute,*/ Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, StyleMaterialModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  title?: string;
  loginResult?: LoginResult;
  form!: FormGroup;

  ngOnInit(): void {
    this.form = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  onSubmit() {
    var loginRequest = <LoginRequest>{};
    loginRequest.email = this.form.controls['email'].value;
    loginRequest.password = this.form.controls['password'].value;

    this.authService
      .login(loginRequest)
      .subscribe(result => {
        console.log(result);
        this.loginResult = result;
        if (result.success) {
          this.router.navigate(["/"]);
        }
      }, error => {
        console.log(error);
        if (error.status == 401) {
          this.loginResult = error.error;
        }
      });
  }

  getErrors(
    control: AbstractControl,
    displayName: string,
    customMessages: { [key: string]: string } | null = null
  ): string[] {
    return this.authService.getErrors(control, displayName, customMessages);
  }

  constructor(
/*    private activatedRoute: ActivatedRoute,*/
    private router: Router,
    private authService: AuthService  ) { }
}
