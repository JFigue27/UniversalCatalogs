import Head from 'next/head';
import Link from 'next/link';
import { Drawer, AppBar, Toolbar, IconButton, Typography, Menu, MenuItem, Grid, Tabs, Tab } from '@material-ui/core';
import { Button, Icon, Fab } from '@material-ui/core';
import Dialog from '../../widgets/Dialog';
import AuthService from '../../core/AuthService';
import Login from '../../widgets/Login';
import '../../styles.scss';

export default class extends React.Component {
  pages = [
    ///start:slot:pages<<<
    { href: '/items', label: 'Items' }
    ///end:slot:pages<<<
  ];

  state = {
    top: false,
    left: false,
    bottom: false,
    right: false,
    auth: true,
    anchorEl: null,
    currentTab: 0,
    loginOpen: false
  };

  classes = {
    root: {
      flexGrow: 1
    },
    grow: {
      flexGrow: 1
    },
    menuButton: {
      marginLeft: -12,
      marginRight: 20
    }
  };

  async componentDidMount() {
    AuthService.fillAuthData();
    AuthService.OpenLogin = this.openLoginDialog;
    if (!AuthService.auth) {
      AuthService.OpenLogin();
    }
    //  else {
    //   AuthService.auth.user = await userService.GetByUserName(AuthService.auth.user.UserName);
    //   localStorage.setItem('authData', JSON.stringify(AuthService.auth));
    // }
  }
  openLoginDialog = () => {
    this.setState({ loginOpen: true });
  };
  closeLoginDialog = () => {
    this.setState({ loginOpen: false });
  };

  logout = () => {
    AuthService.logout();
    this.openLoginDialog();
  };

  toggleDrawer = (side, open) => () => {
    this.setState({
      [side]: open
    });
  };
  handleMenu = event => {
    this.setState({ anchorEl: event.currentTarget });
  };
  handleClose = () => {
    this.setState({ anchorEl: null });
  };

  handleTabsChange = (event, newValue) => {
    this.setState({ currentTab: newValue });
  };

  render() {
    let classes = this.classes;
    const { auth, anchorEl } = this.state;
    const open = Boolean(anchorEl);

    function LinkTab(props) {
      let { href } = props;
      return (
        <Link href={href} passHref>
          <Tab component='a' {...props} />
        </Link>
      );
    }

    return (
      <div>
        <style global jsx>
          {`
            body {
              margin: 0;
            }
          `}
        </style>
        <Head>
          <title>Universal Catalogs</title>
          <meta charSet='utf-8' />
          <meta name='viewport' content='minimum-scale=1, initial-scale=1, width=device-width, shrink-to-fit=no' />
          <link rel='stylesheet' href='/static/styles/bootstrap.css' />
          <link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Roboto:300,400,500' />
          <link rel='stylesheet' href='https://fonts.googleapis.com/icon?family=Material+Icons' />
          <link rel='icon' type='image/x-icon' href='/static/favicon.ico' />
          <style>
            {`
              body {
                font-family: 'Roboto';
              }
              `}
          </style>
        </Head>
        <Dialog open={this.state.loginOpen} onClose={this.closeLoginDialog} fullScreen={true} actionsOff>
          {() => <Login onCloseLogin={this.closeLoginDialog} />}
        </Dialog>
        <AppBar position='fixed'>
          <Toolbar>
            <IconButton color='inherit' onClick={this.toggleDrawer('right', true)}>
              <Icon>menu</Icon>
            </IconButton>
            <Typography variant='h6' color='inherit'>
              Universal Catalogs
            </Typography>
            <Grid item xs />

            <Tabs variant='standard' value={this.state.currentTab} onChange={this.handleTabsChange}>
              {this.pages.map(page => {
                return <LinkTab key={page.label} label={page.label} href={page.href} />;
              })}
            </Tabs>

            <IconButton aria-owns={open ? 'menu-appbar' : undefined} aria-haspopup='true' onClick={this.handleMenu} color='inherit'>
              <Icon>account_circle</Icon>
            </IconButton>
            <Menu
              id='menu-appbar'
              anchorEl={anchorEl}
              anchorOrigin={{
                vertical: 'top',
                horizontal: 'right'
              }}
              transformOrigin={{
                vertical: 'top',
                horizontal: 'right'
              }}
              open={open}
              onClose={this.handleClose}
            >
              <MenuItem onClick={this.handleClose}>Profile</MenuItem>
              <MenuItem onClick={this.logout}>Logout</MenuItem>
            </Menu>
          </Toolbar>
        </AppBar>
        <Drawer anchor='left' open={this.state.right} onClose={this.toggleDrawer('right', false)}>
          <div tabIndex={0} role='button' onClick={this.toggleDrawer('right', false)} onKeyDown={this.toggleDrawer('right', false)}>
            <div style={{ width: 200 }}>Content</div>
          </div>
        </Drawer>
        <Grid container>
          <Grid container direction='column' item xs={12} style={{ padding: 20 }}>
            {this.props.children}
          </Grid>
        </Grid>
      </div>
    );
  }
}
