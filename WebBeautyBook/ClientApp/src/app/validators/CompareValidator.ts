import { FormGroup } from "@angular/forms";

// Define a custom validator function that compares two form control values
export function CompareValidator(nameControlFirst: string, nameControlSecond: string, invert:boolean = false) {

  // Return a validator function that takes a FormGroup as input
  return (formGroup: FormGroup) => {
    // Get references to the two form controls we want to compare
    const control = formGroup.controls[nameControlFirst];
    const matchingControl = formGroup.controls[nameControlSecond];
    // Check if the second control already has an error of 'confirmedValidator'
    if (matchingControl.errors && !matchingControl.errors.confirmedValidator) return;
    // Compare the values of the two controls
    if (!invert && control.value !== matchingControl.value) {
      matchingControl.setErrors({ confirmedValidator: true });
    } else if(invert && control.value === matchingControl.value){
      matchingControl.setErrors({ confirmedValidator: true });
    } else {
      matchingControl.setErrors(null);
    }
  };
}
