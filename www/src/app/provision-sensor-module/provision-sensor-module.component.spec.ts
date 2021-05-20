import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvisionSensorModuleComponent } from './provision-sensor-module.component';

describe('ProvisionSensorModuleComponent', () => {
  let component: ProvisionSensorModuleComponent;
  let fixture: ComponentFixture<ProvisionSensorModuleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProvisionSensorModuleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProvisionSensorModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
