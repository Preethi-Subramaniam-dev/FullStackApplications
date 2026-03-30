import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddedAsset, Asset, AssigneeAsset, EditAsset } from '../models/asset';

@Injectable({
  providedIn: 'root',
})

export class AssetService {

  private apiUrl = 'https://localhost:62693/api/Asset';
  private http = inject(HttpClient);

  getAssets(): Observable<Asset[]> {
    return this.http.get<Asset[]>(this.apiUrl + '/all');
  }

  addAsset(asset: AddedAsset): Observable<Asset> {
    return this.http.post<Asset>(this.apiUrl + '/add', asset);
  }

  editAsset(assetId: string, asset: EditAsset): Observable<Asset> {
    return this.http.put<Asset>(`${this.apiUrl}/edit/${assetId}`, asset);
  }

  deleteAsset(assetId: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/delete/${assetId}`);
  }

  assignAsset(assetId: string, assigneeAsset: AssigneeAsset): Observable<Asset> {
    return this.http.put<Asset>(`${this.apiUrl}/assign/${assetId}`, assigneeAsset);
  }
}
