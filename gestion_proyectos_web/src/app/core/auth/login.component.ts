import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    PasswordModule,
    ButtonModule,
    CardModule,
    MessagesModule,
    MessageModule
  ],
  template: `
    <div class="flex align-items-center justify-content-center min-h-screen"
         style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);">
      <p-card [style]="{ width: '420px' }" styleClass="shadow-6">
        <ng-template pTemplate="header">
          <div class="text-center pt-4">
            <i class="pi pi-th-large text-4xl text-primary"></i>
            <h2 class="mt-2 mb-0 text-900">Gestión de Proyectos</h2>
            <p class="text-500 mt-1">Inicia sesión para continuar</p>
          </div>
        </ng-template>

        <form [formGroup]="loginForm" (ngSubmit)="iniciarSesion()">
          <div class="flex flex-column gap-3">
            <!-- Correo electrónico -->
            <div class="flex flex-column gap-1">
              <label for="correo" class="font-semibold text-900">Correo electrónico</label>
              <span class="p-input-icon-left w-full">
                <i class="pi pi-envelope"></i>
                <input id="correo" type="email" pInputText
                       formControlName="correoElectronico"
                       placeholder="correo@ejemplo.com"
                       class="w-full" />
              </span>
              <small class="text-red-500"
                     *ngIf="loginForm.get('correoElectronico')?.touched && loginForm.get('correoElectronico')?.errors?.['required']">
                El correo es obligatorio
              </small>
            </div>

            <!-- Contraseña -->
            <div class="flex flex-column gap-1">
              <label for="password" class="font-semibold text-900">Contraseña</label>
              <p-password id="password"
                          formControlName="password"
                          [toggleMask]="true"
                          [feedback]="false"
                          placeholder="Contraseña"
                          styleClass="w-full"
                          inputStyleClass="w-full">
              </p-password>
              <small class="text-red-500"
                     *ngIf="loginForm.get('password')?.touched && loginForm.get('password')?.errors?.['required']">
                La contraseña es obligatoria
              </small>
            </div>

            <!-- Mensaje de error -->
            <p-message *ngIf="errorMensaje" severity="error" [text]="errorMensaje"></p-message>

            <!-- Botón login -->
            <button pButton type="submit" label="Iniciar Sesión"
                    icon="pi pi-sign-in"
                    [loading]="cargando"
                    [disabled]="loginForm.invalid || cargando"
                    class="w-full">
            </button>
          </div>
        </form>
      </p-card>
    </div>
  `,
  styles: [`
    :host {
      display: block;
    }
  `]
})
export class LoginComponent {
  loginForm: FormGroup;
  cargando = false;
  errorMensaje = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.loginForm = this.fb.group({
      correoElectronico: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  iniciarSesion(): void {
    if (this.loginForm.invalid || this.cargando) return;

    this.cargando = true;
    this.errorMensaje = '';

    this.authService.login(this.loginForm.value).subscribe({
      next: () => {
        this.cargando = false;
        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/proyectos';
        this.router.navigateByUrl(returnUrl);
      },
      error: (err) => {
        this.cargando = false;
        this.errorMensaje = err.error?.error || 'Error al iniciar sesión';
      }
    });
  }
}
