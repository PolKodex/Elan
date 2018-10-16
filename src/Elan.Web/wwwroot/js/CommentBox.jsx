class CommentBox extends React.Component {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>{this.props.data.title}</div>
        );
    }
}
