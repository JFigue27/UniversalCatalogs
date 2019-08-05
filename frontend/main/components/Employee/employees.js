import React from 'react';
import { withRouter } from 'next/router';
import { withSnackbar } from 'notistack';
import { NoSsr, Typography, Grid } from '@material-ui/core';
import SearchBox from '../../widgets/Searchbox';
import Pagination from 'react-js-pagination';
import ListContainer from '../../core/ListContainer';
import { Table } from '@material-ui/core';
import { TableHead } from '@material-ui/core';
import { TableBody } from '@material-ui/core';
import { TableRow } from '@material-ui/core';
import { TableCell } from '@material-ui/core';
import { Button } from '@material-ui/core';
import { Icon } from '@material-ui/core';
import { InputBase } from '@material-ui/core';

import Dialog from '../../widgets/Dialog';
import Employee from './employee.js';
import { AppBar, Toolbar } from '@material-ui/core';

import EmployeeService from './employee.service';
///start:slot:dependencies<<<///end:slot:dependencies<<<

const service = new EmployeeService();
const defaultConfig = {
  service
  ///start:slot:config<<<///end:slot:config<<<
};

class EmployeesList extends ListContainer {
  constructor(props, config) {
    Object.assign(defaultConfig, config);
    super(props, defaultConfig);
    ///start:slot:ctor<<<///end:slot:ctor<<<
  }

  componentDidMount() {
    console.log('List did mount');
    this.load();

    ///start:slot:didMount<<<///end:slot:didMount<<<
  }

  AFTER_LOAD = baseList => {
    console.log('AFTER_LOAD');
    ///start:slot:afterLoad<<<///end:slot:afterLoad<<<
  };

  AFTER_CREATE = instance => {
    console.log('AFTER_CREATE', instance);

    ///start:slot:afterCreate<<<
    this.openDialog(instance);
    ///end:slot:afterCreate<<<
  };

  AFTER_CREATE_AND_CHECKOUT = entity => {
    console.log('AFTER_CREATE_AND_CHECKOUT', entity);
    ///start:slot:afterCreateCheckout<<<///end:slot:afterCreateCheckout<<<
  };

  AFTER_REMOVE = () => {
    console.log('AFTER_REMOVE');
    ///start:slot:afterRemove<<<///end:slot:afterRemove<<<
  };

  ON_OPEN_ITEM = item => {
    console.log('ON_OPEN_ITEM', item);

    ///start:slot:onOpenItem<<<
    this.openDialog(item);
    ///end:slot:onOpenItem<<<
  };

  openDialog = item => {
    this.setState({
      employee: item
    });
  };

  closeDialog = feedback => {
    if (feedback == 'ok') {
      this.refresh();
    }
    this.setState({
      employee: false
    });
  };
  ///start:slot:js<<<///end:slot:js<<<

  render() {
    const { isLoading, baseEntity, baseList, filterOptions } = this.state;

    return (
      <NoSsr>
        <Grid className='container-fluid' container direction='column' item xs={12} style={{ padding: 20 }}>
          <Grid container direction='row'>
            <Grid item xs>
              <Typography variant='h4' className='h4' gutterBottom style={{ textAlign: 'center' }}>
                Employees
              </Typography>
            </Grid>
            <Pagination
              activePage={filterOptions.page}
              itemsCountPerPage={filterOptions.limit}
              totalItemsCount={filterOptions.totalItems}
              pageRangeDisplayed={5}
              onChange={newPage => {
                this.pageChanged(newPage);
              }}
            />
          </Grid>
          <Table className='' size='small'>
            <TableHead>
              <TableRow>
                <TableCell />
                <TableCell>Clock Number</TableCell>
                <TableCell>Name</TableCell>
                <TableCell>Last Name</TableCell>
                <TableCell>Second Last Name</TableCell>
                <TableCell>CURP</TableCell>
                <TableCell>Personal Number</TableCell>
                <TableCell>Time Id Number</TableCell>
                <TableCell>STPS Position</TableCell>
                <TableCell>Area</TableCell>
                <TableCell>Shift</TableCell>
                <TableCell>Job Position</TableCell>
                <TableCell>SupervisedBy</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {baseList &&
                baseList.map(item => (
                  <TableRow key={item.Id}>
                    <TableCell>
                      <Grid container direction='row' className='row' justify='center' alignItems='flex-start' spacing={2}>
                        <Grid item xs>
                          <Button
                            variant='contained'
                            color='default'
                            className=''
                            onClick={event => {
                              this.openItem(event, item);
                            }}
                            size='small'
                          >
                            <Icon>edit</Icon>Open
                          </Button>
                        </Grid>
                      </Grid>
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='text'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'Value')}
                        value={item.Value || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='text'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'Name')}
                        value={item.Name || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='text'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'LastName')}
                        value={item.LastName || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='text'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'SecondLastName')}
                        value={item.SecondLastName || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='text'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'CURP')}
                        value={item.CURP || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='text'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'PersonalNumber')}
                        value={item.PersonalNumber || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='text'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'TimeIdNumber')}
                        value={item.TimeIdNumber || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='text'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'STPSPosition')}
                        value={item.STPSPosition || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='number'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'Area')}
                        value={item.Area || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='number'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'Shift')}
                        value={item.Shift || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='number'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'JobPosition')}
                        value={item.JobPosition || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='number'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'SupervisedBy')}
                        value={item.SupervisedBy || ''}
                        fullWidth
                      />
                    </TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>
        </Grid>
        <Dialog open={!!this.state.employee} onClose={this.closeDialog} draggable title='Employee' okLabel='Save'>
          {dialog => {
            return !isLoading && <Employee dialog={dialog} data={this.state.employee} />;
          }}
        </Dialog>
        <AppBar position='fixed' style={{ top: 'auto', bottom: 0, background: '#333333' }}>
          <Toolbar variant='dense'>
            <SearchBox bindFilterInput={this.bindFilterInput} value={filterOptions.filterGeneral} />
            <Grid item xs />
            <Button
              variant='contained'
              color='default'
              className=''
              onClick={event => {
                this.createInstance(event, {});
              }}
            >
              <Icon>add_circle</Icon>New
            </Button>
          </Toolbar>
        </AppBar>
      </NoSsr>
    );
  }
}

export default withSnackbar(withRouter(EmployeesList));
