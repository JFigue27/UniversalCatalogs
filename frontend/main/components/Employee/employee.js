import React from 'react';
import { withRouter } from 'next/router';
import { NoSsr, Typography, Grid, TextField } from '@material-ui/core';
import FormContainer from '../../core/FormContainer';
import { withSnackbar } from 'notistack';
import { InputBase } from '@material-ui/core';

import EmployeeService from './employee.service';
///start:slot:dependencies<<<///end:slot:dependencies<<<

const service = new EmployeeService();
const defaultConfig = {
  service
  ///start:slot:config<<<///end:slot:config<<<
};

class EmployeeForm extends FormContainer {
  constructor(props, config) {
    Object.assign(defaultConfig, config);
    super(props, defaultConfig);
    ///start:slot:ctor<<<///end:slot:ctor<<<
  }

  componentDidMount() {
    console.log('Form did mount');
    this.load(this.props.data.Id);

    ///start:slot:didMount<<<///end:slot:didMount<<<
  }

  AFTER_LOAD = entity => {
    console.log('AFTER_LOAD', entity);
    ///start:slot:afterLoad<<<///end:slot:afterLoad<<<
  };

  AFTER_CREATE = instance => {
    console.log('AFTER_CREATE', instance);

    ///start:slot:afterCreate<<<///end:slot:afterCreate<<<
  };

  AFTER_CREATE_AND_CHECKOUT = entity => {
    console.log('AFTER_CREATE_AND_CHECKOUT', entity);
    ///start:slot:afterCreateCheckout<<<///end:slot:afterCreateCheckout<<<
  };

  AFTER_SAVE = entity => {
    console.log('AFTER_SAVE', entity);
    const { dialog } = this.props;
    if (dialog) dialog.close('ok');
    ///start:slot:afterSave<<<///end:slot:afterSave<<<
  };

  BEFORE_CHECKIN = () => {
    console.log('BEFORE_CHECKIN');
    ///start:slot:beforeCheckin<<<///end:slot:beforeCheckin<<<
  };

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    const { dialog } = this.props;
    if (dialog) dialog.onOk = this.onDialogOk;

    const { isLoading, baseEntity } = this.state;

    return (
      <NoSsr>
        <Grid className='' container direction='column' item xs={12}>
          <TextField
            type='text'
            label='Clock Number'
            value={baseEntity.Value || ''}
            onChange={event => this.handleInputChange(event, 'Value')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='text'
            label='Name'
            value={baseEntity.Name || ''}
            onChange={event => this.handleInputChange(event, 'Name')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='text'
            label='Last Name'
            value={baseEntity.LastName || ''}
            onChange={event => this.handleInputChange(event, 'LastName')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='text'
            label='Second Last Name'
            value={baseEntity.SecondLastName || ''}
            onChange={event => this.handleInputChange(event, 'SecondLastName')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='text'
            label='CURP'
            value={baseEntity.CURP || ''}
            onChange={event => this.handleInputChange(event, 'CURP')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='text'
            label='Personal Number'
            value={baseEntity.PersonalNumber || ''}
            onChange={event => this.handleInputChange(event, 'PersonalNumber')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='text'
            label='Time Id Number'
            value={baseEntity.TimeIdNumber || ''}
            onChange={event => this.handleInputChange(event, 'TimeIdNumber')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='text'
            label='STPS Position'
            value={baseEntity.STPSPosition || ''}
            onChange={event => this.handleInputChange(event, 'STPSPosition')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='number'
            label='Area'
            value={baseEntity.Area || ''}
            onChange={event => this.handleInputChange(event, 'Area')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='number'
            label='Shift'
            value={baseEntity.Shift || ''}
            onChange={event => this.handleInputChange(event, 'Shift')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='number'
            label='Job Position'
            value={baseEntity.JobPosition || ''}
            onChange={event => this.handleInputChange(event, 'JobPosition')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='number'
            label='SupervisedBy'
            value={baseEntity.SupervisedBy || ''}
            onChange={event => this.handleInputChange(event, 'SupervisedBy')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
        </Grid>
      </NoSsr>
    );
  }
}

export default withSnackbar(withRouter(EmployeeForm));
