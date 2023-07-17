import { FormGroup } from "@angular/forms";

export function CompareValidator(nameConrolFirst: string, nameConrolSecond: string) {

  return (formGroup: FormGroup) => {
    const control = formGroup.controls[nameConrolFirst];
    const matchingControl = formGroup.controls[nameConrolSecond];
    if (matchingControl.errors && !matchingControl.errors.confirmedValidator) {
      return;
    }
    if (control.value !== matchingControl.value) {
      matchingControl.setErrors({ confirmedValidator: true });
    } else {
      matchingControl.setErrors(null);
    }
  };
}