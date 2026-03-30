import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { SoftwareService } from '../../services/software';
import { SoftwareListItem } from '../../models/software';

@Component({
  selector: 'software-page',
  standalone: true,
  imports: [CommonModule, MatTableModule],
  templateUrl: './software.html',
  styleUrl: './software.css',
})
export class SoftwarePage implements OnInit {
  private readonly softwareService = inject(SoftwareService);

  readonly displayedColumns: string[] = ['softwareLicenseID', 'softwareName'];
  dataSource = new MatTableDataSource<SoftwareListItem>([]);
  
    ngOnInit(): void {
    this.softwareService.getSoftware().subscribe(software => {
      this.dataSource.data = software;
      });
    }
}
