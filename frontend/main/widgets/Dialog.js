import React from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import withMobileDialog from '@material-ui/core/withMobileDialog';
import Paper from '@material-ui/core/Paper';
import Draggable from 'react-draggable';

function DraggableDialog(props) {
  return (
    <Draggable
      cancel={
        '.NoDraggable,.MuiGrid-root,.MuiFormControl-root,button,.Person-Chip,.MuiInputBase-input,.filled,.MuiSwitch-root,.MuiList-root,.react-swipeable-view-container,.MuiTable-root'
      }
    >
      <Paper {...props} />
    </Draggable>
  );
}

class DialogWidget extends React.Component {
  onOk = () => {}; //To be defined on children
  close = () => {}; //Overwritten from props.

  render() {
    const { draggable, okLabel, title, onClose, open, maxWidth, fullScreen, actionsOff, children, actions } = this.props;
    this.close = onClose;

    return (
      <Dialog
        fullScreen={fullScreen}
        open={open || false}
        onClose={onClose}
        maxWidth={maxWidth}
        fullWidth={true}
        PaperComponent={draggable && !fullScreen ? DraggableDialog : Paper}
      >
        {title && <DialogTitle>{title}</DialogTitle>}
        <DialogContent dividers={true} style={{ padding: 10 }}>
          {children(this)}
        </DialogContent>

        {!actionsOff &&
          (actions || (
            <DialogActions>
              <Button onClick={onClose} color='primary'>
                Close
              </Button>

              {okLabel && (
                <Button
                  onClick={() => {
                    this.onOk();
                  }}
                  color='primary'
                >
                  {okLabel == true ? 'OK' : okLabel}
                </Button>
              )}
            </DialogActions>
          ))}
      </Dialog>
    );
  }
}

export default withMobileDialog()(DialogWidget);
