import datetime


def calculate_week_number(date_obj: datetime.date) -> int:
    """Return week number using the First Four-Day Week rule starting on Monday."""
    return date_obj.isocalendar().week


def test_year_end_start_week_numbers():
    cases = {
        datetime.date(2015, 12, 31): 53,
        datetime.date(2016, 1, 1): 53,
        datetime.date(2017, 12, 31): 52,
        datetime.date(2018, 1, 1): 1,
        datetime.date(2020, 12, 31): 53,
        datetime.date(2021, 1, 1): 53,
        datetime.date(2024, 12, 31): 1,
        datetime.date(2025, 1, 1): 1,
    }
    for date_obj, expected_week in cases.items():
        assert calculate_week_number(date_obj) == expected_week
