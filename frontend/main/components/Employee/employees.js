import React from 'react';
import { withRouter } from 'next/router';
import { withSnackbar } from 'notistack';
import { NoSsr, Typography, Grid } from '@material-ui/core';
import SearchBox from '../../widgets/Searchbox';
import Pagination from 'react-js-pagination';
import ListContainer from '../../core/ListContainer';

import { Container } from '@material-ui/core';
import { Table } from '@material-ui/core';
import { TableHead } from '@material-ui/core';
import { TableBody } from '@material-ui/core';
import { TableRow } from '@material-ui/core';
import { TableCell } from '@material-ui/core';
import { Paper } from '@material-ui/core';
import { Button } from '@material-ui/core';
import { Icon } from '@material-ui/core';
import { InputBase } from '@material-ui/core';

import Dialog from '../../widgets/Dialog';
import Employee from './employee.js';
import { AppBar } from '@material-ui/core';
import { Toolbar } from '@material-ui/core';

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
    ///start:slot:load<<<
    ///end:slot:load<<<
  }

  AFTER_LOAD = baseList => {
    ///start:slot:afterLoad<<<///end:slot:afterLoad<<<
  };

  AFTER_CREATE = instance => {
    ///start:slot:afterCreate<<<
    this.openDialog('employee', instance);
    ///end:slot:afterCreate<<<
  };

  AFTER_CREATE_AND_CHECKOUT = entity => {
    ///start:slot:afterCreateCheckout<<<///end:slot:afterCreateCheckout<<<
  };

  AFTER_REMOVE(entity) {
    ///start:slot:afterRemove<<<///end:slot:afterRemove<<<
    super.AFTER_REMOVE(entity);
  }

  ON_OPEN_ITEM = item => {
    ///start:slot:onOpenItem<<<
    this.openDialog('employee', item);
    ///end:slot:onOpenItem<<<
  };

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    const { isLoading, baseEntity, baseList, filterOptions, isDisabled } = this.state;

    ///start:slot:render<<<///end:slot:render<<<

    return (
      <NoSsr>
        <Container style={{ padding: 20 }} maxWidth='xl'>
          <Typography variant='h5' className='h5' gutterBottom>
            Employees
          </Typography>
          <Grid container direction='row'>
            <Grid item xs />
            <Pagination
              activePage={filterOptions.page}
              itemsCountPerPage={filterOptions.limit}
              totalItemsCount={filterOptions.itemsCount}
              pageRangeDisplayed={5}
              onChange={newPage => {
                this.pageChanged(newPage);
              }}
            />
          </Grid>
          <Paper style={{ width: '100%', overflowX: 'auto' }}>
            <Table size='small'>
              <TableHead>
                <TableRow>
                  <TableCell></TableCell>
                  <TableCell>SAPNumber</TableCell>
                  <TableCell>Name</TableCell>
                  <TableCell>LastName</TableCell>
                  <TableCell>SecondLastName</TableCell>
                  <TableCell>CURP</TableCell>
                  <TableCell>PersonalNumber</TableCell>
                  <TableCell>TimeIdNumber</TableCell>
                  <TableCell>STPSPosition</TableCell>
                  <TableCell>Area</TableCell>
                  <TableCell>Shift</TableCell>
                  <TableCell>JobPosition</TableCell>
                  <TableCell>SupervisedBy</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {baseList &&
                  baseList.map((item, index) => (
                    <TableRow key={item.Id}>
                      <TableCell>
                        <Grid container direction='row' className='row' justify='center' alignItems='flex-end' spacing={2}>
                          <Grid item xs={12} sm>
                            <Button
                              variant='contained'
                              color='default'
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
                        <Typography>{item.SAPNumber}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.Name}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.LastName}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.SecondLastName}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.CURP}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.PersonalNumber}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.TimeIdNumber}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.STPSPosition}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.Area}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.Shift}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.JobPosition}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography>{item.SupervisedBy}</Typography>
                      </TableCell>
                    </TableRow>
                  ))}
              </TableBody>
            </Table>
          </Paper>
        </Container>
        <Dialog opener={this} id='employee' title='Employee Form' okLabel='Save' maxWidth='md'>
          {dialog => {
            return !isLoading && <Employee dialog={dialog} data={this.state.employee} />;
          }}
        </Dialog>
        <AppBar position='fixed' style={{ top: 'auto', bottom: 0, backgroundColor: '#333333' }}>
          <Toolbar variant='dense'>
            <SearchBox bindFilterInput={this.bindFilterInput} value={filterOptions.filterGeneral} />
            <Grid item xs={12} sm />
            <Button
              variant='contained'
              color='default'
              onClick={event => {
                this.createInstance({});
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
