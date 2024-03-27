import { Component } from "@angular/core";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterLink } from "@angular/router";
import { AuthService } from "../../services/auth.service";
import { RegisterModel } from "../../models/RegisterModel";
import { LoginModel } from "../../models/LoginModel";
import { Router } from "@angular/router";

@Component({
    selector:"register",
    standalone: true,
    templateUrl: "./register.component.html",
    imports: [FormsModule, ReactiveFormsModule, RouterLink]
})
export class RegistrerComponent {
    
    form: FormGroup;
    submitted: boolean = false;
    errorDetails: any | undefined;
    errors: any | undefined;

    constructor(private formBuilder: FormBuilder,
                private authService: AuthService,
                private router: Router) {
        this.form = formBuilder.group({
            firstName: ["", [Validators.required]],
            lastName: ["", [Validators.required]],
            email: ["", [Validators.required, Validators.email]],
            userName: ["", [Validators.required]],
            password: ["", [Validators.required]],
            region: ["", [Validators.required]]
        });
    }

    register() {
        this.submitted = true;

        if (this.form.invalid){
            return;
        }

        this.errors = undefined;
        this.errorDetails = undefined;

        let model = new RegisterModel();
        model.firstName = this.capitalise(this.form.value.firstName);
        model.lastName = this.capitalise(this.form.value.lastName);
        model.email = this.form.value.email;
        model.userName = this.form.value.userName;
        model.password = this.form.value.password;
        model.region = this.form.value.region;

        this.authService.register(model).subscribe({
            next: async () => {
                let loginModel = new LoginModel(model.email, model.password);
                if (await this.authService.login(loginModel)){
                    this.router.navigate(["/"]);
                }
                
            },
            error: (response) => {
                if ("detail" in response.error) {
                    this.errorDetails = JSON.parse(response.error.detail);
                    console.log(this.errorDetails);
                }

                if ("errors" in response.error){
                    this.errors = response.error.errors;
                }

                console.log(response);
            }
        });

    }

    private capitalise(str: string) : string {
        return str.charAt(0).toUpperCase() + str.slice(1);
    }
}