import { Injectable } from '@angular/core';
import {AbstractControl } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  getErrors(
    control: AbstractControl,
    displayName: string,
    customMessages: { [key: string]: string } | null = null
  ): string[] {
    var errors: string[] = [];
    Object.keys(control.errors || {}).forEach((key) => {
      switch (key) {
        case 'required':
          errors.push(displayName.concat(customMessages?.[key] ?? ' is required.'));
          break;
        case 'pattern':
          errors.push(displayName.concat(customMessages?.[key] ?? ' contains invalid characters.'));
          break;
        case 'isDupeField':
          errors.push(displayName.concat(customMessages?.[key] ?? ' already exists: please choose another.'));
          break;
        default:
          errors.push(displayName.concat(' is invalid.'));
          break;
      }
    });
    return errors;
  }

  constructor() { }
}
