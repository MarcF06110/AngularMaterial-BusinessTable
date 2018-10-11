import { Component, ViewChild, AfterViewInit, ElementRef, EventEmitter, OnInit } from '@angular/core';
import { PersonsService } from '../_services/persons.service';
import { IPerson } from '../_entities/person';
import { MatTableDataSource, MatPaginator, MatSort, MatSnackBar } from '@angular/material';
import { merge, fromEvent } from 'rxjs';
import { startWith, tap, debounceTime, distinctUntilChanged, filter } from 'rxjs/operators';
import { switchMap } from 'rxjs/operators';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements AfterViewInit {

  dataSource = new MatTableDataSource();
  resultLength = 0;
  persons: IPerson[] = [];
  isWorking = true;
  selection = new SelectionModel<IPerson>(false, [], true);

  editingPerson: IPerson;
  mustReload = new EventEmitter();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('searchBox') searchBox: ElementRef;

  constructor(
    private personsService: PersonsService,
    private snackBar: MatSnackBar) { }

  ngAfterViewInit(): void {
    // copy original datas before editing
    this.selection.onChange
      .pipe(
        filter(v => v.added.length === 1),
        tap(v => this.editingPerson = Object.assign({}, v.added[0]))
      ).subscribe();

    // launch request when changing text in searchbox
    fromEvent(this.searchBox.nativeElement, 'keyup')
      .pipe(
        debounceTime(100),
        distinctUntilChanged(),
        switchMap(() => {
          this.paginator.pageIndex = 0;
          this.isWorking = true;
          return this.loadPersons();
        }),
        tap(data => {
          this.isWorking = false;
          this.resultLength = data.count;
          this.dataSource.data = data.items;
        })
      ).subscribe();

    // go back to first page when changing sort
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    // query when changing sort or changing page or manual query
    merge(this.sort.sortChange, this.paginator.page, this.mustReload)
      .pipe(
        switchMap(() => {
          this.isWorking = true;
          return this.loadPersons();
        }),
        tap(data => {
          this.resultLength = data.count;
          this.dataSource.data = data.items;
          this.isWorking = false;
        })
      ).subscribe();

    this.mustReload.emit();
  }


  delete(id: number) {
    this.isWorking = true;
    this.personsService.delete(id).subscribe(result => {
      this.isWorking = false;
      this.snackBar.open('Person deleted');
      this.selection.clear();
      this.mustReload.emit();
    });
  }

  save(person: IPerson) {
    this.isWorking = true;
    this.personsService.update(this.editingPerson).subscribe(result => {
      this.isWorking = false;
      this.snackBar.open('Person saved');
      this.selection.clear();
      this.mustReload.emit();
    });
  }

  personCreated(person: IPerson) {
    this.mustReload.emit();
  }

  private loadPersons() {
    return this.personsService.loadAll(
      this.searchBox.nativeElement.value,
      this.paginator.pageIndex,
      this.paginator.pageSize,
      this.sort.active,
      this.sort.direction);
  }
}
