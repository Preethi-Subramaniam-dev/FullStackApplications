import { AfterViewInit, Component, OnInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AssetService } from '../../services/asset';
import { EmployeeService } from '../../services/employee';
import { Employee } from '../../models/employee';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { SoftwareLicense, AddedAsset, Asset, AssigneeAsset, EditAsset } from '../../models/asset';

type PopupMode = 'add' | 'edit' | 'assign' | null;

@Component({
  selector: 'assets',
  imports: [MatTableModule, MatSortModule, MatToolbarModule, CommonModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule, MatButtonModule, FormsModule],
  templateUrl: './assets.html',
  styleUrl: './assets.css',
})

export class Assets implements OnInit, AfterViewInit {
  readonly displayedColumns: string[] = [
    'assetId',
    'serialNumber',
    'name',
    'statusName',
    'employee',
    'warrantyProvider',
    'installedSoftwares',
    'Actions',
  ];

  private readonly assetService = inject(AssetService);
  private readonly employeeService = inject(EmployeeService);
  private readonly liveAnnouncer = inject(LiveAnnouncer);

  dataSource = new MatTableDataSource<Asset>();
  popupMode: PopupMode = null;
  popupAsset: Asset | null = null;
  addAssetModel: AddedAsset = this.getEmptyAddAsset();
  availableEmployees: Employee[] = [];
  selectedEmployeeId = '';

  ngOnInit(): void {
    this.loadAssets();
    this.loadEmployees();
  }

  @ViewChild(MatSort) sort!: MatSort;

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  announceSortChange(sortState: Sort): void {
    if (sortState.direction) {
      this.liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this.liveAnnouncer.announce('Sorting cleared');
    }
  }

  loadAssets(): void {
    this.assetService.getAssets().subscribe(assets => {
      this.dataSource.data = assets;
    });
  }

  loadEmployees(): void {
    this.employeeService.getEmployees().subscribe(employees => {
      this.availableEmployees = employees;
    });
  }
  
  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  onAddAsset(): void {
    this.popupMode = 'add';
    this.popupAsset = null;
    this.addAssetModel = this.getEmptyAddAsset();
  }

  onEditAsset(asset: Asset): void {
    this.popupMode = 'edit';
    this.popupAsset = { ...asset };
  }

  saveEdit(): void {
    if (!this.popupAsset) {
      return;
    }
    const editAsset: EditAsset = {
      name: this.popupAsset.name,
      serialNumber: this.popupAsset.serialNumber,
      warrantyName: this.popupAsset.warrantyName,
      employeeName: this.popupAsset.employeeName,
    };

    this.assetService.editAsset(this.popupAsset.assetId, editAsset).subscribe(() => {
      // Defer UI state changes to the next microtask to keep change detection stable.
      queueMicrotask(() => {
        this.closePopup();
        this.loadAssets();
      });
    });
  }

  saveAdd(): void {
    this.assetService.addAsset(this.addAssetModel).subscribe(() => {
      queueMicrotask(() => {
        this.closePopup();
        this.loadAssets();
      });
    });
  }

  onAssignAsset(asset: Asset): void {
    this.popupMode = 'assign';
    this.popupAsset = { ...asset };
    this.selectedEmployeeId = '';
  }

  saveAssignment(): void {
    if (!this.popupAsset || !this.selectedEmployeeId) {
      return;
    }

    const selectedEmployee = this.availableEmployees.find(emp => emp.employeeId === this.selectedEmployeeId);
    if (selectedEmployee) {
      this.popupAsset.employeeName = selectedEmployee.employeeName;
    }

    const assignedAsset: AssigneeAsset = {
      assetId: this.popupAsset.assetId,
      employeeId: this.selectedEmployeeId,
    };

    this.assetService.assignAsset(this.popupAsset.assetId, assignedAsset).subscribe(() => {
      // Defer UI state changes to the next microtask to keep change detection stable.
      queueMicrotask(() => {
        this.closePopup();
        this.loadAssets();
      });
    });
  }

  closePopup(): void {
    this.popupMode = null;
    this.popupAsset = null;
    this.selectedEmployeeId = '';
    this.addAssetModel = this.getEmptyAddAsset();
  }

  onDeleteAsset(asset: Asset): void {
    this.assetService.deleteAsset(asset.assetId).subscribe(() => {
      this.loadAssets();
    });
  }

  trackByEmployeeId(_: number, employee: Employee): string {
    return employee.employeeId;
  }

  trackByLicenseName(_: number, software: SoftwareLicense): string {
    return software.licenseName;
  }

  get isAddMode(): boolean {
    return this.popupMode === 'add';
  }

  get isEditMode(): boolean {
    return this.popupMode === 'edit';
  }

  get isAssignMode(): boolean {
    return this.popupMode === 'assign';
  }

  private getEmptyAddAsset(): AddedAsset {
    return {
      name: '',
      serialNumber: '',
      warrantyName: '',
    };
  }
}