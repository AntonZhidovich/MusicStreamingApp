import { Component } from "@angular/core";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { LoginModel } from "../../models/LoginModel";
import { AuthService } from "../../services/auth.service";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";

@Component({
    selector: "auth",
    standalone: true,
    templateUrl: "./signin.component.html",
    imports: [FormsModule, ReactiveFormsModule, RouterLink]
})
export class SigninComponent {

    form: FormGroup;
    showPassword: boolean = false;
    showInvalidLogin: boolean = false;
    returnUrl = "";

    constructor(private authService: AuthService,
                private router: Router,
                formBuilder: FormBuilder,
                route: ActivatedRoute) {
                    this.form = formBuilder.group({
                        email: ["", Validators.required],
                        password: ["", [Validators.required]]
                    })

                    route.queryParams.subscribe(params => {
                        this.returnUrl = params["returnUrl"];
                    });
                }

    async login() {
        if (this.form.invalid){
            return;
        }
        
        let loginModel = new LoginModel(this.form.value.email, this.form.value.password);

        let loggedIn = await this.authService.login(loginModel);

        if (loggedIn){
            if (!this.returnUrl || this.returnUrl.length == 0) {
                this.returnUrl = "/";
            }

            this.router.navigate([this.returnUrl]);
        }
        else {
            this.showInvalidLogin = true;
        }
    }
}