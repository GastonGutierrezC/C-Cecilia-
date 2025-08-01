import { Output, EventEmitter } from '@angular/core';
import {Component, inject, OnInit} from '@angular/core';
import {MatBottomSheetRef} from '@angular/material/bottom-sheet';
import {MatTab, MatTabGroup} from '@angular/material/tabs';
import {MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {ProviderModel} from '../../models/provider';
import {ProviderService} from '../../service/provider-service';
import {
  MatList,
  MatListItem, MatListItemIcon,
  MatListItemLine,
  MatListItemTitle,
  MatListSubheaderCssMatStyler
} from '@angular/material/list';
import {MatButton} from '@angular/material/button';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatIcon} from '@angular/material/icon';
import { SuccessDialogComponent } from '../../dialog/success-dialog/success-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-provider',
  imports: [
    MatTabGroup,
    MatTab,
    MatFormField,
    MatLabel,
    MatInput,
    MatLabel,
    MatFormField,
    MatList,
    MatListItem,
    MatListItemTitle,
    MatListItemLine,
    MatListSubheaderCssMatStyler,
    MatButton,
    ReactiveFormsModule,
    MatListItemIcon,
    MatIcon,
  ],
  templateUrl: './provider.component.html',
  styleUrl: './provider.component.scss'
})
export class ProviderComponent implements OnInit{
  @Output() providerUpdated = new EventEmitter<void>();
  readonly dialog = inject(MatDialog);

  private bottomSheetRef =
    inject<MatBottomSheetRef<ProviderComponent>>(MatBottomSheetRef);
  providers: ProviderModel[] = [];
  providerService = inject(ProviderService)
  editId = 0

  createProviderForm = new FormGroup({
    name: new FormControl<string>('',  [Validators.required]),
    objectiveMount: new FormControl<number>(0,  [Validators.required]),
  })

  editProviderForm = new FormGroup({
    name: new FormControl<string>('',  [Validators.required]),
    objectiveMount: new FormControl<number>(0,  [Validators.required]),
  })
  editMode: boolean = false;
  ngOnInit() {
    this.providerService.getProviders().subscribe((providers) => {
      this.providers = providers;
      this.providerUpdated.emit();
    })
  }

  createProvider() {
    if (this.createProviderForm.value.name !== undefined
    && this.createProviderForm.value.name !== null
      && this.createProviderForm.value.objectiveMount !== undefined
      && this.createProviderForm.value.objectiveMount !== null
      && this.createProviderForm.valid
    )
    this.providerService.createProvider(
      {
        name: this.createProviderForm.value.name,
        objetiveMount: this.createProviderForm.value.objectiveMount,
      }
    ).subscribe({
      next: (res) => {
        if (res) {
          this.dialog.open(SuccessDialogComponent, {
            data: {
              title: '¡Proveedor creado!',
              message: `El proveedor "${this.createProviderForm.value.name}" se registró correctamente`,
              icon: 'check_circle',
              buttonText: 'Aceptar'
            }
          });

          this.createProviderForm.reset();
          Object.keys(this.createProviderForm.controls).forEach(key => {
            this.createProviderForm.get(key)?.setErrors(null);
          });

          this.providerUpdated.emit();

          this.providerService.getProviders().subscribe((providers) => {
            this.providers = providers;
          });
        }
      },
      error: (err) => {
        console.error('Error al crear proveedor:', err);
      }
    })
  }

  editProvider() {
    console.log(this.editId)
    if (this.editProviderForm.value.name !== undefined
      && this.editProviderForm.value.name !== null
      && this.editProviderForm.value.objectiveMount !== undefined
      && this.editProviderForm.value.objectiveMount !== null
      && this.editId != 0
      && this.editProviderForm.valid
    )
    {
      this.providerService.editProvider(
        {
          id: this.editId,
          name: this.editProviderForm.value.name,
          objetiveMount: this.editProviderForm.value.objectiveMount,
        }
      ).subscribe({
        next: (res) => {
          if (res) {
            this.providerService.getProviders().subscribe((providers) => {
              this.providers = providers;
              this.editMode = false
              this.providerUpdated.emit();
            })
          }
        },
        error: (err) => {
          console.error('Error al editar proveedor:', err);
        }
      })
    }
  }

  entryEditMode(provider: ProviderModel) {
    this.editProviderForm.controls.name.setValue(provider.name);
    this.editProviderForm.controls.objectiveMount.setValue(provider.objetiveMount);
    this.editId = provider.id
    this.editMode = true;
  }
}
