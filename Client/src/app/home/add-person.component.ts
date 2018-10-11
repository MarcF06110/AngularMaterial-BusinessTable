import { Component, OnInit, EventEmitter, ViewChild, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ErrorStateMatcher, MatExpansionPanel } from '@angular/material';
import { IPerson } from '../_entities/person';
import { PersonsService } from '../_services/persons.service';

@Component({
  selector: 'app-add-person',
  templateUrl: './add-person.component.html',
  styleUrls: ['./add-person.component.scss']
})
export class AddPersonComponent implements OnInit {

  form: FormGroup;
  matcher = new ErrorStateMatcher();
  isWorking = false;
  @Output() personCreated = new EventEmitter<IPerson>();
  @ViewChild('panel') panel: MatExpansionPanel;

  constructor(
    private formBuilder: FormBuilder,
    private personsService: PersonsService) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      age: [18, [Validators.required, Validators.min(18), Validators.max(60)]]
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    const person: IPerson = {
      id: 0,
      firstName: <string>this.form.get('firstName').value,
      lastName: <string>this.form.get('lastName').value,
      age: <number>this.form.get('age').value
    };

    this.isWorking = true;
    this.personsService.create(person).subscribe(result => {
      this.isWorking = false;
      this.personCreated.emit(result);
      this.form.patchValue({
        firstName: '',
        lastName: '',
        age: 18
      });
      this.panel.close();
    });
  }

}
