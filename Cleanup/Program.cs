namespace Cleanup
{
    internal class Program
    {
        private const double TargetChangeTime = 1;

        private double _previousTargetSetTime;
        private bool _isTargetSet;
        private dynamic _lockedCandidateTarget;
        private dynamic _lockedTarget;
        private dynamic _target;
        private dynamic _previousTarget;
        private dynamic _activeTarget;
        private dynamic _targetInRangeContainer;

        public void CleanupTest(Frame frame)
        {
            ResetInvalidTargerts();

            _isTargetSet = false;
            // Sets _activeTarget field
            TrySetActiveTargetFromQuantum(frame);

            // If target exists and can be targeted, it should stay within Target Change Time since last target change
            var shouldStayOnTarget = ShouldStayOnTarget;
            
            // Если мы знаем что в TrySetActiveTargetFromQuantum не меняется переменная IsTargetSet,
            // тогда можно убрать оператор || _isTargetSet, в данной ситуации мы гарантируем
            // что isTargetSet сохранит свое значение если shuouldStayOnTarget = false
            _isTargetSet = shouldStayOnTarget || _isTargetSet;

            _previousTarget = _target;

            TrySetNewTarget();

            var isNewTarget = _isTargetSet && _previousTarget != _target;
            if (isNewTarget)
            {
                ResetTargetTime();
            }
            else
            {
                ResetTarget();
            }

            TargetableEntity.Selected = _target;
        }

        private void ResetTarget()
        {
            _target = null;
        }

        private void ResetTargetTime()
        {
            _previousTargetSetTime = Time.time;
        }

        private void TrySetNewTarget()
        {
            if (!_isTargetSet)
            {
                _target = _lockedTarget?.CanBeTarget ??
                          _activeTarget?.CanBeTarget ??
                          _targetInRangeContainer.GetTarget();

                _isTargetSet = _target != null;
            }
        }

        private dynamic ShouldStayOnTarget
        {
            get
            {
                var shouldStayOnTarget = _target?.CanBeTarget &&
                                         Time.time - _previousTargetSetTime < TargetChangeTime;
                return shouldStayOnTarget;
            }
        }

        private void ResetInvalidTargerts()
        {
            if (!_lockedCandidateTarget?.CanBeTarget)
            {
                _lockedCandidateTarget = null;
            }

            if (!_lockedTarget?.CanBeTarget)
            {
                _lockedTarget = null;
            }
        }

        // MORE CLASS CODE
    }
}