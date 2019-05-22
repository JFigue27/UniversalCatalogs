import React from 'react';
import PropTypes from 'prop-types';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import withMobileDialog from '@material-ui/core/withMobileDialog';
import Paper from '@material-ui/core/Paper';
import Draggable from 'react-draggable';

function DraggableDialog(props) {
  return (
    <Draggable>
      <Paper {...props} />
    </Draggable>
  );
}

class DialogWidget extends React.Component {
  render() {
    const { draggable, okLabel, children, title, onClose, ok, open, maxWidth, fullScreen } = this.props;
    console.log(draggable);
    return (
      <Dialog
        fullScreen={fullScreen}
        open={open}
        onClose={onClose}
        maxWidth={maxWidth}
        fullWidth={true}
        PaperComponent={draggable && !fullScreen ? DraggableDialog : Paper}
      >
        {title && <DialogTitle>{title}</DialogTitle>}
        <DialogContent>{children}</DialogContent>
        <DialogActions>
          <Button onClick={onClose} color='primary'>
            Close
          </Button>
          <Button onClick={ok} color='primary'>
            {okLabel || 'OK'}
          </Button>
        </DialogActions>
      </Dialog>
    );
  }
}

export default withMobileDialog()(DialogWidget);
