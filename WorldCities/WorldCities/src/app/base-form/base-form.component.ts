import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, AbstractControl } from '@angular/forms';

@Component({
  //selector: 'app-base-form',
  //standalone: true,
  //imports: [CommonModule],
  template: ``,
  //styles: []
})
export abstract class BaseFormComponent {

  // the form model
  form!: FormGroup

  ngOnInit(): void {
  }

  getErrors(
    control: AbstractControl,
    displayName: string,
  ): string[] {
    var errors: string[] = [];
    Object.keys(control.errors || {}).forEach((key) => {
      switch (key) {
        case 'required':
          errors.push('${displayName} is required.');
          break;
        case 'pattern':
          errors.push('${displayName} contains invalid characters.');
          break;
        case 'isDupeField':
          errors.push('${displayName} already exists: please choose another.'); break;
        default:
          errors.push('${displayName} is invalid.');
          break;
      }
    });
    return errors;
  }

  constructor() { }
}