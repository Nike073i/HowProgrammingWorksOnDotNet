namespace HowProgrammingWorksOnDotNet.Patterns.Builder;

public record Member(Guid Id, string Name)
{
    public static readonly Member Guest = new(Guid.Empty, "GUEST");
}

public class Meeting
{
    public IReadOnlyCollection<Member> Members { get; private set; }
    public Member Host { get; private set; }

    private Meeting(Member host, List<Member> members)
    {
        Host = host;
        Members = members;
    }

    // Compile error. Нельзя указать дефолтным значением - объект, который будет создан в рантайме.
    // public static Meeting CreateMeeting(Member host, params Member[] people = [Member.Guest]) { }

    public class Builder
    {
        private readonly List<Member> _members = [];
        private Member? _host;

        public Builder WithHost(Member host)
        {
            _host = host;
            return this;
        }

        public Builder AddMember(Member member)
        {
            _members.Add(member);
            return this;
        }

        // В качестве дефолтного значения может быть объект в рантайме
        public Meeting CreateMeeting()
        {
            if (_host is null)
                throw new InvalidOperationException();

            if (!_members.Any())
                _members.Add(Member.Guest);

            return new Meeting(_host, _members);
        }
    }
}
