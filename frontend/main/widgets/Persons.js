import React from 'react';
import { Paper, Chip, Avatar, NoSsr } from '@material-ui/core';
import Select from './Select';

export default class Persons extends React.Component {
  state = {
    selected: []
  };

  adapterIn = value => {
    let persons = typeof value == 'string' ? JSON.parse(value || '[]') : value || [];
    if (persons.length) {
      const { keyProp = 'Id', labelProp = 'Value', emailProp = 'Email' } = this.props;
      return persons.map(person => {
        return {
          value: person[keyProp],
          label: person[labelProp],
          email: person[emailProp],
          avatar: person[labelProp]
            .split(' ')
            .map(word => word.substring(0, 1).toUpperCase())
            .join('')
            .slice(0, 2)
        };
      });
    } else {
      return [];
    }
  };

  adapterOut = selected => {
    const { keyProp = 'Id', labelProp = 'Value', emailProp = 'Email', json } = this.props;

    let result = selected.map(person => {
      let adapted = {};
      adapted[keyProp] = person.value;
      adapted[labelProp] = person.label;
      adapted[emailProp] = person.email;
      return adapted;
    });

    if (json) return JSON.stringify(result);

    return result;
  };

  componentDidMount() {
    this.setState({
      selected: this.adapterIn(this.props.value),
      allOptions: this.adapterIn(this.props.options)
    });
  }

  componentDidUpdate(prevProps) {
    const { options: prevOptions, value: prevValue } = prevProps;
    const { options, value } = this.props;

    if (!prevOptions && options) {
      this.setState({
        allOptions: this.adapterIn(options)
      });
    }

    if ((!prevValue && value) || prevValue != value) {
      this.setState({
        selected: this.adapterIn(value)
      });
    }
  }

  onAdd = selected => {
    let arr = this.state.selected;
    arr.push(selected);
    this.setState({
      selected: arr
    });

    this.onChange(this.adapterOut(arr));
  };

  onRemove = index => {
    this.state.selected.splice(index, 1);
    this.setState({
      selected: this.state.selected
    });
    this.onChange(this.adapterOut(this.state.selected));
  };

  render() {
    const { onChange } = this.props;
    const { selected } = this.state;
    this.onChange = onChange;

    return (
      <>
        <NoSsr>
          <Paper style={{ minHeight: 30, margin: '10px 0' }}>
            {
              (selected && this,
              selected.map((item, index) => {
                return (
                  <Chip
                    key={item.value}
                    color='primary'
                    variant='outlined'
                    label={item.label}
                    className='Person-Chip'
                    onDelete={() => this.onRemove(index)}
                    avatar={<Avatar>{item.avatar}</Avatar>}
                  />
                );
              }))
            }
            <Select
              options={(this.state.allOptions || []).filter(opt => !this.state.selected.some(s => s.value == opt.value))}
              onChange={this.onAdd}
              placement={this.props.placement || 'top'}
              placeholder='Select Persons.'
            />
          </Paper>
        </NoSsr>
      </>
    );
  }
}
