import React from 'react';
import { withRouter } from 'next/router';
import { NoSsr, Typography, Grid, TextField } from '@material-ui/core';
import FormContainer from '../../core/FormContainer';
import { withSnackbar } from 'notistack';

import { Container } from '@material-ui/core';
import { Paper } from '@material-ui/core';
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
    ///start:slot:load<<<
    this.load(this.props.data.Id);
    ///end:slot:load<<<
  }

  AFTER_LOAD = entity => {
    ///start:slot:afterLoad<<<///end:slot:afterLoad<<<
  };

  AFTER_CREATE = instance => {
    ///start:slot:afterCreate<<<///end:slot:afterCreate<<<
  };

  AFTER_CREATE_AND_CHECKOUT = entity => {
    ///start:slot:afterCreateCheckout<<<///end:slot:afterCreateCheckout<<<
  };

  AFTER_SAVE = entity => {
    const { dialog } = this.props;
    if (dialog) dialog.close('ok');

    ///start:slot:afterSave<<<///end:slot:afterSave<<<
  };

  BEFORE_CHECKIN = async entity => {
    ///start:slot:beforeCheckin<<<///end:slot:beforeCheckin<<<
    return entity;
  };

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    const { dialog } = this.props;
    if (dialog) dialog.onOk = this.onDialogOk;

    const { isLoading, isDisabled, baseEntity } = this.state;

    ///start:slot:render<<<///end:slot:render<<<

    return (
      <NoSsr>
        <Container style={{ padding: 20 }} maxWidth='md'>
          <Paper className='Sheet ' elevation={10}>
            <>
              <Grid container direction='row' className='row' justify='center' alignItems='flex-end' spacing={2}></Grid>
              <TextField
                type='text'
                label='SAPNumber'
                value={baseEntity.SAPNumber || ''}
                onChange={event => this.handleInputChange(event, 'SAPNumber')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='text'
                label='Name'
                value={baseEntity.Name || ''}
                onChange={event => this.handleInputChange(event, 'Name')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='text'
                label='LastName'
                value={baseEntity.LastName || ''}
                onChange={event => this.handleInputChange(event, 'LastName')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='text'
                label='SecondLastName'
                value={baseEntity.SecondLastName || ''}
                onChange={event => this.handleInputChange(event, 'SecondLastName')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='text'
                label='CURP'
                value={baseEntity.CURP || ''}
                onChange={event => this.handleInputChange(event, 'CURP')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='text'
                label='PersonalNumber'
                value={baseEntity.PersonalNumber || ''}
                onChange={event => this.handleInputChange(event, 'PersonalNumber')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='text'
                label='TimeIdNumber'
                value={baseEntity.TimeIdNumber || ''}
                onChange={event => this.handleInputChange(event, 'TimeIdNumber')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='text'
                label='STPSPosition'
                value={baseEntity.STPSPosition || ''}
                onChange={event => this.handleInputChange(event, 'STPSPosition')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='number'
                label='Area'
                value={baseEntity.Area || ''}
                onChange={event => this.handleInputChange(event, 'Area')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='number'
                label='Shift'
                value={baseEntity.Shift || ''}
                onChange={event => this.handleInputChange(event, 'Shift')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='number'
                label='JobPosition'
                value={baseEntity.JobPosition || ''}
                onChange={event => this.handleInputChange(event, 'JobPosition')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
              <TextField
                type='number'
                label='SupervisedBy'
                value={baseEntity.SupervisedBy || ''}
                onChange={event => this.handleInputChange(event, 'SupervisedBy')}
                style={{ textAlign: 'left' }}
                margin='dense'
                fullWidth
              />
            </>
          </Paper>
        </Container>
      </NoSsr>
    );
  }
}

export default withSnackbar(withRouter(EmployeeForm));
